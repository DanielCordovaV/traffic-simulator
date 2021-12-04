# traffic-simulator

Graphical simulation of traffic, representing the output of a multi-agent system.

---

### Contributors

- [Andres Eduardo Nowak de Anda A01638430](https://github.com/andresnowak)
- [Daniel Cordova Verdugo A01630123](https://github.com/DanielCordovaV)
- [Jorge Alejandro López Sosa A01637313](https://github.com/jloftw)
- [Eduardo Esteva Camacho A01632202](https://github.com/A01632202)
- [Sebastián Márquez Álvarez A01632483](https://github.com/SebastianMarAlv)

### Link a video de como se instala y corre todo
[video](https://youtu.be/6beJN9lNgJY)

## Requirements
### Server:
- flask
- agentpy

Para poder importar websockets y agentpy se instala de la manera
```pip3 install -r requirements.txt```

### Unity:
- [NavMeshComponents](https://github.com/Unity-Technologies/NavMeshComponents) **ya viene en el unity package**

## Run program
Meterse a la carpeta backend y correr el archivo
```python server.py```
este es el que crea el servidor de http para unity

Y para el unity primero tiene que crear un nuevo proyecto de universal render pipeline y ya que lo tenga solo necesita importar el unity package y meterse a la escena de simulation y cambiar en el script por su url de servidor local si no quiere usar el de ibm.