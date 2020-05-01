<template>
  <div class="container">
    <div class="word-grid" v-if="showGrid">
      <div  v-for="die in dice" v-bind:key="die.position">
        <dice-face 
          :position="die.position" 
          :value="die.value" 
          :rotation="die.rotation" 
          :shouldUnderline="die.shouldUnderline"
          :shouldShake="isShaking" />
      </div>
    </div>
  </div>
  
</template>

<script lang="ts">
import Vue from "vue";
import DiceFace from "./DiceFace.vue";

export default Vue.extend({
  components: {
    DiceFace
  },
  props: {
    dice: { type:Array, default:() => [] },
    isShaking: {type:Boolean, default: () => false }
  },
  data: () => {
    return {
    }
  },
  computed: {
    showGrid: function() {
      if(this.dice == null)
        return false;

      if(this.dice.length !== 16)
        return false;

      return true;
    }
  }
  
})
</script>

<style lang="scss" scoped>
.container {
  // margin: auto;
}

.word-grid {
  display: grid;
  grid-template-columns: 20% 20% 20% 20%;
  grid-template-rows: 20% 20% 20% 20%;
  grid-gap: 12px;
  // margin: auto;
}
</style>