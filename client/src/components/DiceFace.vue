<template>
  <h1 
    :class="{ 
      'requires-underline' : shouldUnderline,
      'rotate-90' : rotation === 90, 
      'rotate-180' : rotation === 180,
      'rotate-270' : rotation === 270, 
    }">
    {{ value }}
  </h1>
</template>

<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  props: { 
    position: { type: Number },
    value: {type: String },
    rotation: {type: Number },
    shouldUnderline: {type: Boolean },
  }
})
</script>

<style>
.requires-underline {
  text-decoration: underline;
}

.rotate-90 {
  transform: rotate(90deg);
}

.rotate-180 {
  transform: rotate(180deg);
}

.rotate-270 {
  transform: rotate(270deg);
}
</style>