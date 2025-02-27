<template>
  <div id="plotlyGraph" ref="plotlyGraph"></div>
</template>

<script>
import Plotly from 'plotly.js-dist';

//A function that changes the y-axis will need to be added when the dropdowns work

export default {
  name: 'graphBox',
  mounted() {
    this.renderPlot();
  },
  methods: {
    renderPlot() {
      if (typeof Plotly !== 'undefined') {
        var trace1 = {
          x: [0],
          y: [0],
          type: 'scatter'
        };

        var trace2 = {
          x: [1],
          y: [2],
          type: 'scatter'
        };

        var trace3 = {
          x: [2],
          y: [4],
          type: 'scatter'
        };

        var trace4 = {
          x: [0, 1, 2, 3, 4, 5, 6, 7, 8],
          y: [0, 1, 2, 3, 4, 5, 6, 7, 8],
          type: 'scatter'
        };

        var trace5 = {
          x: [3],
          y: [6],
          type: 'scatter'
        };

        var trace6 = {
          x: [0],
          y: [0],
          type: 'scatter'
        };

        var trace7 = {
          x: [0],
          y: [0],
          type: 'scatter'
        };

        var trace8 = {
          x: [0],
          y: [0],
          type: 'scatter'
        };
        var data = [trace1, trace2, trace3, trace4, trace5, trace6, trace7, trace8];

        var layout = {
          xaxis: {
            type: 'log',
            autorange: true,
          },
          yaxis: {
            type: 'log',
            autorange: true,
          }
        };

        var config = {
          responsive: true,
          displayModeBar: true,
          modeBarButtonsToRemove: ['sendDataToCloud', 'zoom2d', 'pan2d', 'select2d', 'lasso2d', 'zoomIn2d', 'zoomOut2d',
            'autoScale2d', 'resetScale2d', 'hoverClosestCartesian', 'hoverCompareCartesian',
            'toggleSpikelines'],
          displaylogo: false,// Ensure the Plotly logo is hidden
          
          //if static the picture will not work
          staticPlot: true 
        };

       
        Plotly.newPlot(this.$refs.plotlyGraph, data, layout, config,);
      } else {
        console.error('Plotly is not defined');
      }
    }
  }
};
</script>

<style scoped>
#plotlyGraph {
  width: 100%;
  height: 100%;
  margin-left: 0;
  position: relative;
}
</style>