<template>
  <div class="game-container">
    <countdown
      v-if="showCountdown"
      :secondsPerRound="gameOptions.secondsPerRound"
      :isActive="!shakeBoard"
      :remaining="roundTimeRemaining"
    />

    <word-grid :dice="gridDice" :isShaking="shakeBoard" />
    <p v-if="showRounds">Round: {{ currentRound }}/{{ roundsToPlay }}</p>
    <game-option-selection v-if="showCreateGame" />
    <v-btn color="primary" v-if="showCreateGame" @click="createGame">Create Game</v-btn>
    <v-btn
      class="btn-display"
      color="primary"
      v-if="showNextRound"
      :disabled="!nextStageEnabled"
      @click="nextRound"
    >{{ currentRound | nextRoundText }}</v-btn>
    <v-btn
      color="primary"
      v-if="showFinishGame"
      :disabled="!nextStageEnabled"
      @click="finishGame"
    >Finish Game</v-btn>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import { defineComponent, Ref } from "@vue/composition-api";
import { ref, computed, watch, onMounted } from "@vue/composition-api";

import WordGrid from "./WordGrid.vue";
import Countdown from "./Countdown.vue";
import GameOptionSelection from "./GameOptionSelection.vue";

import EventBus from "../events/eventBus";
import { CountdownCompletedEvent } from "../events/CountdownCompletedEvent";

import { Game, GameState, Dice } from "../models/GameState";
import { GameOptions } from "../models/GameOptions";

import { createHubConnection } from "../api/hubConnections";
import { triggerNewGame, triggerNexRound, triggerFinishGame, retrieveCurrentGame } from "../api/gameApi";

export default defineComponent({
  name: "GameDisplay",
  components: {
    WordGrid,
    Countdown,
    GameOptionSelection
  },
  filters: {
    nextRoundText: function(round: number): string {
      return round > 0 ? "Next Round" : "Let's Begin";
    }
  },
  setup() {
    // state
    const gameState = ref(GameState.NotStarted);
    const gridDice: Ref<Dice[]> = ref([]);
    const shakeBoard = ref(false);
    const roundTimeRemaining = ref(0);
    const currentRound = ref(0);
    const roundsToPlay = ref(0);
    const gameOptions: Ref<GameOptions> = ref({
      numberOfRounds: 5,
      secondsPerRound: 90
    });

    // computed
    const showCreateGame = computed(
      (): boolean =>
        gameState.value === GameState.NotStarted ||
        gameState.value === GameState.Finished
    );

    const showNextRound = computed(
      (): boolean =>
        gameState.value !== GameState.NotStarted &&
        !(currentRound.value === roundsToPlay.value)
    );

    const showFinishGame = computed((): boolean => {
      const inProgress =
        gameState.value !== GameState.NotStarted &&
        gameState.value !== GameState.Finished;
      const playedAllRounds = currentRound.value === roundsToPlay.value;
      return inProgress && playedAllRounds;
    });

    const nextStageEnabled = computed((): boolean => {
      const isMidRound = roundTimeRemaining.value > 0;
      return !isMidRound;
    });

    const showRounds = computed(
      (): boolean =>
        currentRound.value > 0 && gameState.value !== GameState.Finished
    );

    const showCountdown = computed(
      (): boolean =>
        !(
          gameState.value === GameState.NotStarted ||
          gameState.value === GameState.Finished
        )
    );

    const subscribeEventBus = () => {
      EventBus.$on(
        "CountdownCompleted",
        (countdownCompletedEvent: CountdownCompletedEvent) => {
          roundTimeRemaining.value = 0;
        }
      );
      EventBus.$on("GameOptionsUpdated", (opts: GameOptions) => {
        gameOptions.value = { ...opts };
      });
    };

    const setGameState = (game: Game) => {
      gridDice.value = game.grid;
      gameState.value = game.gameState;
      roundsToPlay.value = game.roundsToPlay;
      currentRound.value = game.currentRound;
      roundTimeRemaining.value = game.roundTimeRemaining;
    };

    // initialisation methods
    const initPushNotifications = async () => {
      const connection = createHubConnection();

      connection.on("gameCreatedAsync", (game: Game) => {
        setGameState(game);
      });

      connection.on("shakingBoardAsync", (game: Game) => {
        setGameState(game);
        shakeBoard.value = true;
        roundTimeRemaining.value = gameOptions.value.secondsPerRound;
      });

      connection.on("nextRoundAsync", (game: Game) => {
        setGameState(game);
        shakeBoard.value = false;
      });

      connection.on("gameFinishedAsync", (game: Game) => {
        setGameState(game);
        gridDice.value = [];
      });

      connection.start().catch(err => console.error(err));
    };

    const loadCurrentData = async () => {
      const game: Game = await retrieveCurrentGame();
      setGameState(game);
      gameOptions.value.numberOfRounds = game.roundsToPlay;
      gameOptions.value.secondsPerRound = game.countdownLength;
      
    };

    const createGame = async () => {
      if (gameOptions.value.numberOfRounds === 0) {
        gameOptions.value.numberOfRounds = 5;
      }

      if (gameOptions.value.secondsPerRound === 0) {
        gameOptions.value.secondsPerRound = 90;
      }
    
      await triggerNewGame(gameOptions.value.numberOfRounds, gameOptions.value.secondsPerRound);
    };

    const nextRound = async () => await triggerNexRound();
    const finishGame = async () => await triggerFinishGame();      

    // hooks, setup cannot be async, therefore perform these actions
    // at the mounted stage.
    onMounted(async () => {
      subscribeEventBus();
      await loadCurrentData();
      await initPushNotifications();
    });

    return {
      // state
      gameState,
      gridDice,
      shakeBoard,
      roundTimeRemaining,
      gameOptions,
      currentRound,
      roundsToPlay,

      // computed
      showCreateGame,
      showNextRound,
      nextStageEnabled,
      showFinishGame,
      showRounds,
      showCountdown,

      // methods
      initPushNotifications,
      subscribeEventBus,
      loadCurrentData,
      setGameState,
      createGame,
      nextRound,
      finishGame
    };
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

.countdown-container {
  display: flex;
}
</style>
