<template>
  <div>
    <h3>Number of Rounds</h3>
    <v-select
      class="opts-selection"
      :items="numberOfRoundsOptions"
      v-model="selectedRounds"
      v-on:change="roundsUpdated"
    />

    <h3>Seconds Per Round</h3>
    <v-select
      class="opts-selection"
      :items="secondsPerRoundOptions"
      v-model="selectedSeconds"
      v-on:change="secondsUpdated"
    />

    <v-spacer />
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import EventBus from "../events/eventBus";
import { GameOptions } from "../models/GameOptions";

export default Vue.extend({
  data: () => {
    return {
      selectedSeconds: 90,
      selectedRounds: 5,
      secondsPerRoundOptions: [30, 60, 90, 120],
      numberOfRoundsOptions: [3, 5, 10]
    };
  },
  methods: {
    secondsUpdated: function(seconds: number) {
      this.selectedSeconds = seconds;
      this.publishUpdate();
    },
    roundsUpdated: function(rounds: number) {
      this.selectedRounds = rounds;
      this.publishUpdate();
    },
    publishUpdate: function() {
      EventBus.$emit("GameOptionsUpdated", {
        numberOfRounds: this.selectedRounds,
        secondsPerRound: this.selectedSeconds
      });
    }
  }
});
</script>

<style lang="scss" scoped>
.opts-selection {
  margin-bottom: 20px;
}
</style>