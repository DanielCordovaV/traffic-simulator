import agentpy as ap

from City import Direction


class Road(ap.Agent):
    def setup(self):
        self.directions = []
        self.condition = 0

    def add_directions(self, directions: list[Direction]) -> None:
        self.directions = directions
