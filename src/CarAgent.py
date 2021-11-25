import agentpy as ap
import random

import RoadAgent


class CarAgent(ap.Agent):

    def move(self) -> None:
        directions = []

        neighbour: ap.Agent
        for neighbour in self.city.neighbors(self, distance=0):
            if neighbour.type is RoadAgent:
                directions = neighbour.directions

        if len(directions) > 0:
            direction = random.choice(directions).value
            if not self.check_if_car_in_front(direction):
                self.model.city.move_by(self, direction)

    def check_if_car_in_front(self, direction: tuple[int]) -> bool:
        new_pos = tuple(map(sum, zip(self.model.positions[self], direction)))
        agent: ap.Agent
        for agent in self.model.city.agents[new_pos[0], new_pos[1]]:
            if agent.type is CarAgent:
                return True
        return False
