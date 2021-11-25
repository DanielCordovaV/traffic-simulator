import enum

import agentpy as ap
import CarAgent

class LightColor(enum.Enum):
    GREEN = 'green_light'
    YELLOW = 'yellow_light'
    RED = 'red_light'


class TrafficLightAgent(ap.agent):
    MAX_TIMER = 10 # Max steps per green light



    def setup(self) -> None:
        self.__current_light = LightColor.YELLOW
        self.timer = 0

    def check_for_incoming_cars(self) -> None:
        if self.__current_light == LightColor.YELLOW:
            neighbours: ap.AgentIter = self.model.city.neighbors(self, distance=3)
            neighbour: ap.Agent
            incoming_cars = ap.AgentIter
            for neighbour in neighbours:
                if neighbour.type is CarAgent:
                    incoming_cars
            if incoming_cars is not None:

