interface Game {
  id: string;
  gameState: GameState;
  gridSize: 4;
  grid: Array<Dice>;
  currentRound: number;
  roundsToPlay: number;
  countdownLength: number;
  roundTimeRemaining: number;
}

enum GameState {
  NotStarted,        
  AwaitingNextRound,
  ShakingBoard,
  RoundInProgress,
  Finished
}

interface Dice {
  position: number,
  value: string,
  rotation: number,
  shouldUnderline: boolean
}

export { Game, GameState, Dice };