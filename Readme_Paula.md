#  Production Plan API 
![Molinos](molinos.jpg)

---

##  English Version

### Installation

Clone this repo and install dependencies:

git clone <your_repo_url>
cd <your_repo_folder>
pip install -r requirements.txt


###  Running the Server
Start the Flask API server on port 8888:
python main.py


###  Using the API
Send POST requests to:
http://localhost:8888/productionplan


-------


##  Versión en Español

### Instalación

Clonar y e instalar este repo en tu espacio:

git clone <your_repo_url>
cd <your_repo_folder>
pip install -r requirements.txt


###  Correr en tu servidor
Correr la Flask API server en 8888:
python main.py


###  Llamar a la API
Enviar una POST requests a:
http://localhost:8888/productionplan


###  Notas
El servidor se ejecuta por defecto en el puerto 8888.
La API devuelve una lista de centrales eléctricas con la potencia asignada (en MW).
Para pruebas internas, se ha utilizado el método Flask test_client().
Para uso en producción, ejecuta con app.run(port=8888).

