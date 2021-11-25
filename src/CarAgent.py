import agentpy as ap
import random

from RoadAgent import Road


class Car(ap.Agent):

    def setup(self):
        self.condition = 1

    def move(self) -> None:
        directions = []

        for neighbor in self.city.neighbors(self, distance=0):
            if type(neighbor) is Road:
                directions = neighbor.directions

        if len(directions) > 0:
            direction = random.choice(directions).value

            pos = self.city.positions[self]
            new_pos = tuple(map(sum, zip(pos, direction)))

            if new_pos[0] < self.size[0] and new_pos[0] >= 0 and \
                    new_pos[1] < self.size[1] and new_pos[1] >= 0 and \
                    not self.check_if_car_in_front(new_pos):
                self.city.move_by(self, random.choice(directions).value)
            else:
                self.city.move_to(
                    self, (new_pos[0] % self.size[0], new_pos[1] % self.size[1]))

    def check_if_car_in_front(self, new_pos: tuple) -> bool:
        for agent in self.city.agents[new_pos[0]][new_pos[1]]:
            if type(agent) is Car:
                return True

        return False
