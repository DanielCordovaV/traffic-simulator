import agentpy as ap
from .City import Direction


class RoadAgent(ap.Agent):

    def setup(self) -> None:
        self.directions: list = []

    def add_directions(self, directions: list) -> None:
        self.directions.extend(directions)
