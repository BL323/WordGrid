<template>
  <div class="game-container">
    <countdown
      v-if="showCountdown"
      :secondsPerRound="gameOptions.secondsPerRound"
      :isActive="!shakeBoard"
      :remaining="roundTimeRemaining"
    />
    <word-grid :dice="gridDice" :isShaking="shakeBoard" />
    <p v-if="showRounds">Round: {{game.currentRound}}/{{game.roundsToPlay}}</p>
    <game-option-selection v-if="showCreateGame" />
    <v-btn color="primary" v-if="showCreateGame" @click="createGame">Create Game</v-btn>
    <v-btn
      class="btn-display"
      color="primary"
      v-if="showNextRound"
      :disabled="!nextRoundEnabled"
      @click="nextRound"
    >{{ game.currentRound | nextRoundText }}</v-btn>
    <v-btn 
    color="primary" 
    v-if="showFinishGame"
    :disabled="!finishGameEnabled"
    @click="finishGame"
    >Finish Game</v-btn>
  </div>
</template>

<script lang="ts">
import Vue from "vue";

import WordGrid from "./WordGrid.vue";
import Countdown from "./Countdown.vue";
import GameOptionSelection from "./GameOptionSelection.vue";

import { Game, GameState, Dice } from "../models/gameState";
import { GameOptions } from "../models/GameOptions";
import EventBus from "../events/eventBus";
import { CountdownCompletedEvent } from "../events/CountdownCompletedEvent";
import axios from "axios";
import * as signalR from "@microsoft/signalr";

const { VUE_APP_API_HOST, VUE_APP_API_PORT } = process.env;

export default Vue.extend({
  name: "GameDisplay",
  components: {
    WordGrid,
    Countdown,
    GameOptionSelection
  },
  props: {},
  data: () => {
    const dice: Dice[] = [];
    const gameOptions: GameOptions = {
      numberOfRounds: 5,
      secondsPerRound: 90
    };
    return {
      game: {},
      gameState: 0,
      gridDice: dice,
      shakeBoard: false,
      roundTimeRemaining: 0,
      gameOptions: gameOptions
    };
  },
  created: async function() {
    this.subscribeEventBus();
    await this.loadCurrentData();
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
    nextRoundEnabled: function(): boolean {
      const g = this.game as Game;
      const isMidRound = this.roundTimeRemaining > 0;
      return !isMidRound;
    },
    showFinishGame: function(): boolean {
      const inProgress = this.gameState !== 0 && this.gameState !== 4;
      const g = this.game as Game;
      const playedAllRounds = g.currentRound === g.roundsToPlay;
      return inProgress && playedAllRounds;
    },
    finishGameEnabled: function(): boolean {
      const g = this.game as Game;
      const isMidRound = this.roundTimeRemaining > 0;
      return !isMidRound;
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
    subscribeEventBus() {
      EventBus.$on(
        "CountdownCompleted",
        (countdownCompletedEvent: CountdownCompletedEvent) => {
          this.roundTimeRemaining = 0;
        }
      );
      EventBus.$on("GameOptionsUpdated", (gameOptions: GameOptions) => {
        this.gameOptions = gameOptions;
      });
    },
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
        this.roundTimeRemaining = this.gameOptions.secondsPerRound;
      });

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

      connection.start().catch(err => console.error(err));
    },
    async loadCurrentData() {
      const response = await axios.get(
        `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/state`
      );
      const game = response.data as Game;
      this.game = game;
      this.gameState = game.gameState;
      this.gridDice = game.grid;
      this.roundTimeRemaining = game.roundTimeRemaining;
      this.gameOptions.numberOfRounds = game.roundsToPlay;
      this.gameOptions.secondsPerRound = game.countdownLength;
    },
    async createGame() {
      if(this.gameOptions.numberOfRounds === 0) {
        this.gameOptions.numberOfRounds = 5;
      }

    if(this.gameOptions.secondsPerRound === 0) {
        this.gameOptions.secondsPerRound = 90;
      }

      const rounds = `numberOfRounds=${this.gameOptions.numberOfRounds}`;
      const seconds = `secondsPerRound=${this.gameOptions.secondsPerRound }`;
      const response = await axios.get(
        `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/create?${rounds}&${seconds}`
      );
    },
    async nextRound() {
      const response = await axios.get(
        `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/next/round`
      );
    },
    async finishGame() {
      const response = await axios.get(
        `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/finish`
      );
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
  margin: auto;
  width: 280px;
}

.btn-display {
  align-content: center;
}
</style>
