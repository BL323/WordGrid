<template>
  <div class="text-center">
    <v-progress-circular
      class="spinner"
      :rotate="360"
      :size="100"
      :width="15"
      :value="percentageRemaining"
      :color="timerColour"
    >{{ secondsRemaining }}</v-progress-circular>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { defineComponent } from "@vue/composition-api";
import { ref, computed, watch, onMounted } from "@vue/composition-api";

import EventBus from "../events/eventBus";
import { CountdownCompletedEvent } from "../events/CountdownCompletedEvent";

export default defineComponent({
  props: {
    remaining: { type: Number, default: () => 0 },
    isActive: { type: Boolean, default: () => false },
    secondsPerRound: { type: Number, default: () => 90 }
  },
  setup(props) {
    // state
    const interval = ref({});

    // one of these could be computed?
    const percentageRemaining = ref(0);
    const secondsRemaining = ref(0);

    // computed
    const timerColour = computed((): string =>
      secondsRemaining.value <= 15 ? "red" : "primary"
    );

    // watches
    const remainingWatch = watch(
      () => props.remaining,
      (newVal: number) => {
        percentageRemaining.value = (100 / props.secondsPerRound) * newVal;
        secondsRemaining.value = Math.round(newVal);
      }
    );

    // hooks
    onMounted(() => {
      percentageRemaining.value =
        (100 / props.secondsPerRound) * props.remaining;
      secondsRemaining.value = Math.round(props.remaining);

      interval.value = setInterval(() => {
        if (!props.isActive) return;

        if (
          percentageRemaining.value === 0 ||
          percentageRemaining.value < 0 ||
          secondsRemaining.value < 0
        ) {
          percentageRemaining.value = 0;
          secondsRemaining.value = 0;
          return;
        }

        percentageRemaining.value -= (100 / props.secondsPerRound) * 1;
        secondsRemaining.value -= 1;

        if (percentageRemaining.value === 0 || percentageRemaining.value < 0) {
          EventBus.$emit("CountdownCompleted", CountdownCompletedEvent);
        }
      }, 1000);
    });

    return {
      interval,
      percentageRemaining,
      remainingWatch,
      secondsRemaining,
      timerColour
    };
  }
});
</script>

<style lang="scss" scoped>
.spinner {
  margin-top: 15px;
}
</style>