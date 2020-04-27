<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h3>{{ GameState }}</h3>
    <button @click="startGame">Start Game</button>

  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import axios from 'axios';
// import { HubConnection, TransportType } from '@aspnet/signalr';
// import { HubConnection } from "@aspnet/signalr";
import * as signalR from "@microsoft/signalr";
import GameState from "../models/gameState";

export default Vue.extend({
  name: 'HelloWorld',
  props: {
    msg: String,
  },
  data: () => {
      return {
          GameState: {},
      };
  },
  created: async function() {
    await this.LoadCurrentData();
    await this.initPushNotifications();
  },
  methods: {
    initPushNotifications() {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:5001/Hubs/Game")
        .build();

      connection.on("gameStateUpdatedAsync", (gt: GameState) => {
        console.log("Game State Updated!");
        this.GameState = gt;
      });

    connection
        .start()
        .catch(err => console.error(err));
    },
    async LoadCurrentData() {
      const response = await axios.get('https://localhost:5001/Game/State');
      this.GameState = response.data;
    }, 
    async startGame() {
      const response = await axios.get('https://localhost:5001/Game/Start/Round');
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
