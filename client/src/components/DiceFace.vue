<template>
  <div class="dice-face" :class="{'shake-face' : shouldShake }">
    <h1
      :class="{ 
        'requires-underline' : shouldUnderline,
        'rotate-90' : rotation === 90, 
        'rotate-180' : rotation === 180,
        'rotate-270' : rotation === 270
      }"
    >{{ value }}</h1>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
export default Vue.extend({
  props: {
    position: { type: Number },
    value: { type: String },
    rotation: { type: Number },
    shouldUnderline: { type: Boolean },
    shouldShake: { type: Boolean }
  }
});
</script>

<style>
h1 {
  line-height: 50px;
  vertical-align: middle;
  text-align: center;
}

.shake-face {
  /* credit: https://www.w3schools.com/howto/howto_css_shake_image.asp */
  animation: shake 0.5s;
  animation-iteration-count: infinite;
}

@keyframes shake {
  0% {
    transform: translate(1px, 1px) rotate(0deg);
  }
  10% {
    transform: translate(-1px, -2px) rotate(-1deg);
  }
  20% {
    transform: translate(-3px, 0px) rotate(1deg);
  }
  30% {
    transform: translate(3px, 2px) rotate(0deg);
  }
  40% {
    transform: translate(1px, -1px) rotate(1deg);
  }
  50% {
    transform: translate(-1px, 2px) rotate(-1deg);
  }
  60% {
    transform: translate(-3px, 1px) rotate(0deg);
  }
  70% {
    transform: translate(3px, 1px) rotate(-1deg);
  }
  80% {
    transform: translate(-1px, -1px) rotate(1deg);
  }
  90% {
    transform: translate(1px, 2px) rotate(0deg);
  }
  100% {
    transform: translate(1px, -2px) rotate(-1deg);
  }
}

.dice-face {
  border: 1px solid black;
  border-radius: 20%;
  height: 50px;
  width: 50px;
}

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