<script setup lang="ts">
import graphBox from './components/Graph.vue';
import { ref } from 'vue';
import './assets/main.css';
import DropDownComponents from "./components/DropDownComponents.vue";
</script>


<template>
  <link rel="preconnect" href="https://fonts.googleapis.com">
  <link rel="preconnect" href="https://fonts.gstatic.com">
  <link href="https://fonts.googleapis.com/css2?family=Josefin+Sans:ital,wght@0,100..700;1,100..700&display=swap" rel="stylesheet">
  
<body>
  <div id="app">
    <header class="banner">
      <div>
        <h1>Fuel Model Comparer</h1>
      </div>
    </header>

    <div class="graph-container">

      <div class="left">
        <graphBox ref="graphBox"></graphBox>

        <button>Generate</button> <!-- will need action to make it run -->

      </div>
      
      <DropDownComponents />
    </div>
      
  </div>
</body>
<!--Add generate button to run after all inputs have been put in-->  
</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Outfit:wght@100..900&display=swap');

/* main.css or App.vue */
body {
  width: 100%; 
  height: 100%; 
  margin: 0 auto; 
  font-family: "Outfit", sans-serif;
  font-optical-sizing: auto;
  font-style: normal;

}

.banner {
  padding:3px;
  text-align: left;
  background-color: #ebe6e7;
  color: #A21629 ;
  font-size: 1.25em;
  width: 100%;
}


.graph-container {
  padding-top: 10px;
  padding-left: 5px;
  padding-right: 5px;
  height: calc(100% - 20px); 
  display: flex;
  gap: 15px;
  width: 100%;
}

.left {
  width: 70%;
  height: 100%;
  display: flex;
  flex-direction: column;
  gap: 15px;
}

button{
  width:50%;
  background-color: #A21629;
  color: white;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 1em;
}
</style>
