from flask import Flask, render_template, request, jsonify
import json
import logging
import os
import atexit
from server import *
from time import time


app = Flask(__name__, static_url_path='')

# On IBM Cloud Cloud Foundry, get the port number from the environment variable PORT
# When running this app on the local machine, default the port to 8000
port = int(os.getenv('PORT', 8000))

size = (20, 20)
city = City(*size)
city.add_street((4, 0), (4, size[1]), Direction.RIGHT)
city.add_street((0, 4), (size[0], 4), Direction.UP)
city.add_street((8, 0), (8, size[1]), Direction.RIGHT)
city.add_street((0, 8), (size[0], 8), Direction.UP)
# Define parameters
parameters = {
    "size": size,
    "city": city,
    "cars": 35,
    "spawn_points": [(4, 0), (0, 4), (8, 0)],
}

models = {}


@app.route('/')
def home():
    return json.dumps({'message': 'Please enter an id'})


@app.route('/<id>')
def sim(id):
    global models
    try:
        # Register user
        if id not in models.keys():
            models[id] = (CityModel(parameters), time())
            models[id][0].sim_setup()
        # Send current state to user
        return models[id][0].get_frame()
    finally:
        old_keys = []
        for key in models.keys():
            if time() - models[key][1] >= 300:
                old_keys.append(key)
        for key in old_keys:
            del models[key]
    return json.dumps({})


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=port, debug=True)
