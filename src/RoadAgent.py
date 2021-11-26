import agentpy as ap
from City import Direction


class RoadAgent(ap.Agent):

    def setup(self) -> None:
        self.directions: list[Direction] = []

    def add_directions(self, directions: list[Direction]) -> None:
        self.directions.extend(directions)
