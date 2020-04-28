interface Game {
  id: string;
  gameState: GameState;
  gridSize: 4;
  grid: Array<Dice>;
  currentRound: number;
  roundsToPlay: number;
  countDownLength: number;
  countDownTo: any;
}

enum GameState {
  NotStarted,        
  AwaitingNextRound,
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