# web
## Proyecto GUI web realizada con Vue.js la cual consume el CRUD que se encuentra en la API
* vue 2.6.14
* axios 1.6.4
* core-js 3.8.3
* vue-simple-alert 1.1.1
* vuetify 2.6.0
* vuex 3.6.2
## Configuración para consumir API
### main.js
```js
axios.defaults.baseURL = 'http://apifx.somee.com/';
```
## Este proyecto se puede ejecutar desde Visual Studio sin ningún problema o Visual Studio Code con los siguientes comandos
### Instalación de paquetes
```
npm install
```

### Ejecutar
```
npm run serve
```

### Compilar a producción
```
npm run build
```
