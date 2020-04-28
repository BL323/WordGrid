<template>
  <div class="hello">
    <word-grid :dice="gridDice" />
    <button v-if="showCreateGame" @click="createGame">Create Game</button>
    <button v-if="showNextRound" @click="nextRound">Next Round</button>
    <button v-if="showFinishGame" @click="finishGame">Finish Game</button>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";
import { Game, GameState, Dice } from "../models/gameState";
import WordGrid from './WordGrid.vue';

export default Vue.extend({
  name: 'GameDisplay',
  components: {
    WordGrid
  },
  props: {
  },
  data: () => {
      const dice: Dice[] = [];
      return {
          game: {},
          gameState: 0,
          gridDice: dice,
      };
  },
  created: async function() {
    await this.LoadCurrentData();
    await this.initPushNotifications();
  },
  computed: {
    showCreateGame: function(): boolean {
      return this.gameState === 0;
    },
    showNextRound: function(): boolean {
      const inProgress = this.gameState !== 0;
      const g = this.game as Game;
      const playedAllRounds = g.currentRound === g.roundsToPlay;
      return inProgress && !playedAllRounds;
    },
    showFinishGame: function(): boolean {
      const inProgress = this.gameState !== 0;
      const g = this.game as Game;
      const playedAllRounds = g.currentRound === g.roundsToPlay;
      return inProgress && playedAllRounds;
    }
  },
  methods: {
    initPushNotifications() {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:5001/Hubs/Game")
        .build();

      connection.on("gameCreatedAsync", (gt: Game) => {
        console.log("Game Created!");
        this.game = gt;
        this.gameState = gt.gameState;
        this.gridDice = gt.grid;
      });

      connection.on("nextRoundAsync", (gt: Game) => {
        console.log("Next Round Started!");
        this.game = gt;
        this.gameState = gt.gameState;
        this.gridDice = gt.grid;
      });

    connection
        .start()
        .catch(err => console.error(err));
    },
    async LoadCurrentData() {
      const response = await axios.get('https://localhost:5001/game/state');
      this.game = response.data;
      this.gameState = response.data.gameState;
    },
    async createGame() {
      const response = await axios.get('https://localhost:5001/game/create');
    },
    async nextRound() {
      const response = await axios.get('https://localhost:5001/game/next/round');
    },
    async finishGame() {
      throw new Error("not yet implemented: requires game to finish");
    }
  }
});
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
