<template>
  <v-app>
    <v-app-bar
      app
      color="primary"
      dark
    >
      <h2>Word Grid</h2>

      <v-spacer></v-spacer>

    </v-app-bar>

    <v-content>
      <game-display />
    </v-content>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import GameDisplay from "./components/GameDisplay.vue";

export default Vue.extend({
  name: 'App',

  components: {
    GameDisplay,
  },

  data: () => ({
    //
  }),
});
</script>
