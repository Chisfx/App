<template>
  <v-app id="inspire">
    <v-main>
      <v-container class="">
        <user />
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
  import User from './components/User'
  export default {
    name: 'Users',

    components: {
      User,
    },
  }
</script>