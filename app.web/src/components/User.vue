<template>
  <v-layout>
    <v-flex>
      <v-data-table
    :headers="headers"
    :items="users"
    sort-by="id"
    class="elevation-1"
  >
    <template v-slot:top>
      <v-toolbar
        flat
      >
        <v-toolbar-title>Users</v-toolbar-title>
        <v-divider
          class="mx-4"
          inset
          vertical
        ></v-divider>
        <v-spacer></v-spacer>
        <v-dialog
          v-model="dialog"
          max-width="500px"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="primary"
              dark
              class="mb-2"
              v-bind="attrs"
              v-on="on"
            >
              New User
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>
            <v-card-text>
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.id"
                      label="ID" 
                      :readonly="true"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.name"
                      label="Name"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.email"
                      label="Email"
                    ></v-text-field>
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.age"
                      label="Age"
                    ></v-text-field>
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="blue darken-1"
                text
                @click="close"
              >
                Cancel
              </v-btn>
              <v-btn
                color="blue darken-1"
                text
                @click="save"
              >
                Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="dialogDelete" max-width="500px">
          <v-card>
            <v-card-title class="text-h5">Are you sure you want to delete this item?</v-card-title>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="closeDelete">Cancel</v-btn>
              <v-btn color="blue darken-1" text @click="deleteItemConfirm">OK</v-btn>
              <v-spacer></v-spacer>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item.actions="{ item }">
      <v-icon
        small
        class="mr-2"
        @click="editItem(item)"
      >
        mode
      </v-icon>
      <v-icon
        small
        @click="deleteItem(item)"
      >
        delete
      </v-icon>
    </template>
    <template v-slot:no-data>
      
    </template>
  </v-data-table>
    </v-flex>
  </v-layout>

</template>
<script>
  import axios from 'axios'
  export default {
    data: () => ({
      users:[],
      dialog: false,
      dialogDelete: false,
      headers: [
        { text: 'ID', value: 'id' },
        { text: 'Name', value: 'name' },
        { text: 'Email', value: 'email' },
        { text: 'Age', value: 'age' },
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      editedItem: {
        id: 0,
        name: '',
        email: '',
        age: ''
      },
      defaultItem: {
        id: 0,
        name: '',
        email: '',
        age: ''
      },
    }),

    computed: {
      formTitle () {
        return this.editedItem.id > 1 ? 'Edit User' : 'New User'
      },
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
      dialogDelete (val) {
        val || this.closeDelete()
      },
    },

    created () {
      this.userlist();
    },

    methods: {
      userlist () {
        let me = this;
        axios.get('api/user/getall').then(function(response){
          me.users = response.data;        
        }).catch(function(error){
          console.log(error);
          if (error.message) {
            me.$alert(error.message,'','error');
          } else {
            me.$alert('Error','','error');
          }
        });
      },
      editItem (item) {
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        this.editedItem = Object.assign({}, item)
        this.dialogDelete = true
      },

      deleteItemConfirm () {
        let me = this;
        axios.delete(`api/user/delete/${me.editedItem.id}`).then(function(response){
          me.closeDelete()
          me.userlist();
          me.$alert('Deleted item','','success');
        }).catch(function(error){
          console.log(error);
          if (error.message) {
            me.$alert(error.message,'','error');
          } else {
            me.$alert('Error','','error');
          }
        });
      },

      close () {
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
        })
      },

      closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
        })
      },

      save () {
        let me = this;
        if (me.editedItem.id > 0) {        
          axios.put('api/user/put',me.editedItem).then(function(response){
            me.close()
            me.userlist();
            me.$alert('Updated item','','success');
          }).catch(function(error){
            console.log(error);
            if (error.message) {
              me.$alert(error.message,'','error');
            } else {
              me.$alert('Error','','error');
            }
          });
        } else {
          axios.post('api/user/post',me.editedItem).then(function(response){
            me.close()
            me.userlist();
            me.$alert('Saved item','','success');
          }).catch(function(error){
            console.log(error);
            if (error.message) {
              me.$alert(error.message,'','error');
            } else {
              me.$alert('Error','','error');
            }
          });
        }       
      },
    },
  }
</script>