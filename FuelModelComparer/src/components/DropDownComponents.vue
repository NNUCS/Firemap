<template>
  <div class="dropDowns">
    <label for="comparers" class="toolTipComparer">
      Choose a Comparer:
      <span class="tooltip-text">Select a comparison method to evaluate data</span>
    </label>
    <!-- comparers -->
    <v-select
      v-model="selectedComparer"
      :items="comparers"
      item-text="outputDisplay"
      item-value="outputRefID"
      id="comparers"
      density="compact"
  ></v-select>


    <label for="humidity" class="toolTipComparer">
      Choose Humidity:
      <span class="tooltip-text">Select a humidity to evaluate data</span>
    </label>
    <v-select  id="humidity" density="compact">
      <option>high</option>
      <option>medium</option>
      <option>low</option>
    </v-select>

    <label for="treecover" class="toolTipComparer">
      Choose Treecover:
      <span class="tooltip-text">Select a treecover to evaluate data</span>
    </label>
    <v-select id="treecover" density="compact"></v-select>

    <label for="" class="toolTipComparer">
      Choose Fuel Model:
      <span class="tooltip-text">Select up to 8 fuel model to compare</span>
      </label>
          
      <div class="models-container">
            
        <div class="modelsL">
            <v-select density="compact"></v-select>
            <v-select density="compact"></v-select>
            <v-select density="compact"></v-select>
            <v-select density="compact"></v-select>
        </div>
        
        <div class="modelsR">
            <v-select density="compact"></v-select>
            <v-select density="compact"></v-select>
            <v-select density="compact"></v-select>
            <v-select density="compact"></v-select>
        </div>
    
      </div>

  </div>

</template>


<script>
import { ref, onMounted } from 'vue';

export default {
  name: 'DropDownComponents',
  setup() {
    const comparers = ref([]);
    const selectedComparer = ref(null);
    const fetchedData = ref({});

// Fetch the data for comparers from the middleware
const fetchComparersData = async () => {
  try {
    const response = await fetch('http://nnucsadmin-001-site2.atempurl.com/Services/fuelModelComparer/Drop1'); // middleware URL
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    const data = await response.json();
    console.log('Fetched comparers data:', data); // Log fetched data to console
    comparers.value = data; 
    fetchedData.value = data; // Store fetched data for testing
  } catch (error) {
    console.error('Error fetching comparers data:', error);
  }
};    

const combineParameters = () => {
  //combine all parameters into one object
  const firemapData = {
      Iteration: null,
      ROS: null,
      FL: null,
      HPA: null,
      WAF: null,
      ModelNumber: null,
    };

    //serialize objects into json
  const jsonString = JSON.stringify(firemapData);

  //create string content object for the http request
  const content = {
      method: 'POST',
      headers: {
      'Content-Type': 'application/json',
      },
      body: jsonString,
    };

  //add URL for the web service
    fetch('http://nnucsadmin-001-site2.atempurl.com/Services/FuelModelComparer/GetFiremapDataParms', content)
      .then((response) => response.json())
      .then((data) => {
        console.log('Firemap Data:', data);

        //send back the deserialized json as Firemap Data
        fetchedData.value = data;
      })
      .catch((error) => {
        console.error('Error fetching Firemap Data:', error);
      });
  };

  onMounted(() => {
    fetchComparersData();
    combineParameters();
    });

  return {
    comparers,
    selectedComparer,
    fetchedData,
  };
}
};



</script>

<style scoped>
/* Add your styles here */
/*NEED TO FIX SPACING */

.dropDowns {
  width: 45%;
  padding: 2px 2px; /* Reduced top and bottom padding */
  display: flex;
  flex-direction: column;
  gap: 0px;
}


.models-container {
  display: flex;
  justify-content: space-between;
}

.modelsL, .modelsR {
  display: flex;
  flex-direction: column;
  gap: 10px;
  width: 48%; /* Adjust the width of the columns */
}

.toolTipComparer {
  position: relative;
  cursor: pointer;
  color:white;
}

.tooltip-text {
  visibility: hidden;
  width: 200px;
  background-color: #807f7f;
  color: #fff;
  text-align: center;
  border-radius: 5px;
  padding: 5px;
  position: absolute;
  z-index: 1;
  bottom: 100%; /* Position above the label */
  left: 50%;
  transform: translateX(-50%);
  opacity: 0;
  transition: opacity 0.3s;
}

.toolTipComparer:hover .tooltip-text {
  visibility: visible;
  opacity: 1;
}
</style>

