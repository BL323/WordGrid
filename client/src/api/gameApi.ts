import axios from "axios";
import { Game } from "@/models/GameState";
const { VUE_APP_API_HOST, VUE_APP_API_PORT } = process.env;

const triggerNewGame = async (
  numberOfRounds: number,
  secondsPerRound: number
) => {
  const rounds = `numberOfRounds=${numberOfRounds}`;
  const seconds = `secondsPerRound=${secondsPerRound}`;

  const response = await axios.get(
    `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/create?${rounds}&${seconds}`
  );
};

const triggerNexRound = async () => {
  const response = await axios.get(
    `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/next/round`
  );
};

const triggerFinishGame = async () => {
  const response = await axios.get(
    `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/finish`
  );
};

const retrieveCurrentGame = async (): Promise<Game> => {
  const response = await axios.get(
    `https://${VUE_APP_API_HOST}:${VUE_APP_API_PORT}/game/state`
  );
  const game = response.data as Game;
  return game;
};

export {
  triggerNewGame,
  triggerNexRound,
  triggerFinishGame,
  retrieveCurrentGame
};
