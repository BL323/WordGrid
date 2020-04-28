<template>
  <div class="home">
    <GameDisplay msg="Welcome to Your Vue.js App"/>
  </div>
</template>

<script>
// @ is an alias to /src
import GameDisplay from '@/components/GameDisplay.vue'

export default {
  name: 'Home',
  components: {
    GameDisplay
  }
}
</script>
