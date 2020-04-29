<template>
  <div class="game-container">
    <countdown v-if="showCountdown" :remaining="game.roundTimeRemaining" />
    <word-grid :dice="gridDice" :isShaking="shakeBoard" />
    <p v-if="showRounds">Round: {{game.currentRound}}/{{game.roundsToPlay}}</p>
    <v-btn color="primary" v-if="showCreateGame" @click="createGame">Create Game</v-btn>
    <v-btn class="btn-display"  color="primary" v-if="showNextRound" @click="nextRound">{{ game.currentRound | nextRoundText }}</v-btn>
    <v-btn color="primary" v-if="showFinishGame" @click="finishGame">Finish Game</v-btn>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
import * as signalR from "@microsoft/signalr";
import { Game, GameState, Dice } from "../models/gameState";

import WordGrid from './WordGrid.vue';
import Countdown from './Countdown.vue';

const { VUE_APP_API_HOST, VUE_APP_API_PORT } = process.env;

export default Vue.extend({
  name: 'GameDisplay',
  components: {
    WordGrid,
    Countdown
  },
  props: {
  },
  data: () => {
      const dice: Dice[] = [];
      return {
          game: {},
          gameState: 0,
          gridDice: dice,
          shakeBoard: false,
      };
  },
  created: async function() {
    await this.LoadCurrentData();
    await this.initPushNotifications();
  },
  filters: {
    nextRoundText: function(round: number): string {
      return round > 0 ? "Next Round" : "Let's Begin";
    }
  },
  computed: {
    showCreateGame: function(): boolean {
      return this.gameState === 0 || this.gameState === 4;
    },
    showNextRound: function(): boolean {
      const inProgress = this.gameState !== 0;
      const g = this.game as Game;
      const playedAllRounds = g.currentRound === g.roundsToPlay;
      return inProgress && !playedAllRounds;
    },
    showFinishGame: function(): boolean {
      const inProgress = this.gameState !== 0 && this.gameState !== 4;
      const g = this.game as Game;
      const playedAllRounds = g.currentRound === g.roundsToPlay;
      return inProgress && playedAllRounds;
    },
    showRounds: function(): boolean {
      const g = this.game as Game;
      return g.currentRound > 0 && g.gameState !== 4;
    },
    showCountdown: function(): boolean {
      return !(this.gameState === 0 || this.gameState === 4);
    }
  },
  methods: {
    initPushNotifications() {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl(`https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/Hubs/Game`)
        .build();

      connection.on("gameCreatedAsync", (gt: Game) => {
        this.game = gt;
        this.gameState = gt.gameState;
        this.gridDice = gt.grid;
      });

      connection.on("shakingBoardAsync", (gt: Game) => {
        this.game = gt;
        this.gameState = gt.gameState;
        this.gridDice = gt.grid;
        this.shakeBoard = true;
      })

      connection.on("nextRoundAsync", (gt: Game) => {
        this.game = gt;
        this.gameState = gt.gameState;
        this.gridDice = gt.grid;
        this.shakeBoard = false;
      });

      connection.on("gameFinishedAsync", (gt: Game) => {
        this.game = gt;
        this.gameState = gt.gameState;
        this.gridDice = [];
      });

    connection
        .start()
        .catch(err => console.error(err));
    },
    async LoadCurrentData() {
      const response = await axios.get(`https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/state`);
      this.game = response.data;
      this.gameState = response.data.gameState;
      this.gridDice = response.data.grid;
    },
    async createGame() {
      const response = await axios.get(`https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/create`);
    },
    async nextRound() {
      const response = await axios.get(`https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/next/round`);
    },
    async finishGame() {
      const response = await axios.get(`https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/finish`);
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

.game-container {
  margin: 0;
  position: absolute;
  top: 50%;
  left: 50%;
  -ms-transform: translate(-50%, -50%);
  transform: translate(-50%, -50%);
}

.btn-display {
  // margin-left: 90px;
  align-content: center;
}
</style>
