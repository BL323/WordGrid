<template>
    <div class="text-center">
    <v-progress-circular
      :rotate="360"
      :size="100"
      :width="15"
      :value="value"
      :color="timerColor"
    >
      {{ secondsRemaining }}
    </v-progress-circular>
 
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import EventBus from "../events/eventBus";
import { CountdownCompletedEvent } from "../events/CountdownCompletedEvent";

export default Vue.extend({
  props: {
    remaining: { type: Number },
    isActive: { type: Boolean },
    secondsPerRound: { type: Number }
  },
   data () {
      return {
        interval: {},
        value: 0,
        secondsRemaining: 0,
        countdownActive: false,
      }
    },
    computed: {
      timerColor: function(): string {
        if(this.secondsRemaining <= 15)
          return "red";

        return "primary";
      }
    },
    watch: {
      remaining: function(newVal: number) {
        this.value = (100 / this.secondsPerRound) * newVal;
        this.secondsRemaining = Math.round(newVal);
      },
      isActive: function(newVal: boolean) {
        this.countdownActive = newVal;
      }
    },
    mounted () {
        this.value = (100 / this.secondsPerRound) * this.remaining;
        this.secondsRemaining = Math.round(this.remaining);
        this.countdownActive = this.isActive;

      this.interval = setInterval(() => {
        if(!this.countdownActive)
          return;

        if (this.value === 0 || this.value < 0) {
          this.value = 0;
          this.secondsRemaining = 0;
          return;
        }

        this.value -= (100 / this.secondsPerRound) * 1;
        this.secondsRemaining -= 1;

        if(this.value === 0 || this.value < 0) {
          EventBus.$emit('CountdownCompleted', CountdownCompletedEvent);
        }

      }, 1000)
    },
})
</script>

<style lang="scss" scoped>

.v-progress-circular {
  margin: 1rem;
}

</style>