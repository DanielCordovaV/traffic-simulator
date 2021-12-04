import agentpy as ap
from random import choice
from .TrafficLightAgent import LightColor


class CarAgent(ap.Agent):
    def check_for_traffic_light(self, direction: tuple) -> bool:
        new_pos = self.__get_new_pos(direction)
        agent: ap.Agent
        for agent in self.model.grid.agents[new_pos]:
            if agent.type == 'TrafficLightAgent':
                if direction == agent.direction.value and agent.get_current_light() == LightColor.RED:
                    return False
        return True

    def check_if_can_move(self, direction: tuple) -> bool:
        grid_width, grid_height = self.model.size
        new_pos_x, new_pox_y = self.__get_new_pos(direction)
        if new_pos_x < grid_width and new_pox_y < grid_height:
            return True
        return False

    def select_next_direction(self) -> tuple:
        directions = []
        neighbour: ap.Agent
        for neighbour in self.model.grid.neighbors(self, distance=0):
            if neighbour.type == 'RoadAgent':
                directions = neighbour.directions
        if len(directions) > 0:
            direction = choice(directions).value
            self.direction = direction
            return direction

    def move(self, direction: tuple) -> None:
        if not self.__check_if_car_in_front(direction):
            self.model.grid.move_by(self, direction)

    def __get_new_pos(self, direction: tuple) -> tuple:
        pos: tuple = self.model.grid.positions[self]
        return tuple(map(sum, zip(pos, direction)))

    def __check_if_car_in_front(self, direction: tuple) -> bool:
        new_pos = self.__get_new_pos(direction)
        agent: ap.Agent
        for agent in self.model.grid.agents[new_pos]:
            if agent.type == 'CarAgent':
                return True
        return False
