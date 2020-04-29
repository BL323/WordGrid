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
export default Vue.extend({
  props: {
    remaining: { type: Number }
  },
   data () {
      return {
        interval: {},
        value: 0,
        secondsRemaining: 0
      }
    },
    computed: {
      timerColor: function(): string {
        if(this.secondsRemaining <= 10)
          return "red";

        return "primary";
      }
    },
    watch: {
      remaining: function(newVal: number) {
        this.value = (100 / 60) * newVal;
        this.secondsRemaining = Math.round(newVal);
      }
    },
    mounted () {
      this.interval = setInterval(() => {
        if (this.value === 0 || this.value < 0) {
          this.value = 0;
          this.secondsRemaining = 0;
          return;
        }

        this.value -= (100 / 60) * 1;
        this.secondsRemaining -= 1;
      }, 1000)
    },
})
</script>

<style lang="scss" scoped>

.v-progress-circular {
  margin: 1rem;
}

</style>