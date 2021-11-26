import agentpy as ap
import random


class CarAgent(ap.Agent):
    def setup(self):
        self.condition = 1

    def move(self) -> None:
        directions = []

        neighbour: ap.Agent
        for neighbour in self.model.grid.neighbors(self, distance=0):
            if neighbour.type == "RoadAgent":
                directions = neighbour.directions

        if len(directions) > 0:
            direction = random.choice(directions).value
            if not self.check_if_car_in_front(direction):
                self.model.grid.move_by(self, direction)

    def check_if_car_in_front(self, direction: tuple[int, int]) -> bool:
        pos: tuple[int, int] = self.model.grid.positions[self]

        new_pos = tuple(
            map(sum, zip(pos, direction)))

        gridWidth = self.model.size[0]
        gridHeight = self.model.size[1]

        agent: ap.Agent
        for agent in self.model.grid.agents[new_pos[0] % gridWidth, new_pos[1] % gridHeight]:
            if agent.type == "CarAgent":
                return True

        return False
