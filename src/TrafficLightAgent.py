import enum
import agentpy as ap
import CarAgent
from City import Direction


class LightColor(enum.Enum):
    GREEN = 'green_light'
    YELLOW = 'yellow_light'
    RED = 'red_light'


class TrafficLightAgent(ap.agent):
    MAX_TIMER = 10 # Max steps per green light
    there_is_main = False

    def setup(self, orientation: Direction) -> None:
        self.__current_light = LightColor.YELLOW
        self.timer = 0
        self.is_main = False
        self.got_main = False
        main_traffic_light: TrafficLightAgent
        self.orientation = orientation

    def get_current_light(self) -> LightColor:
        return self.__current_light


    def set_as_main_light(self, agent: ap.Agent) -> None:
        self.__current_light == LightColor.GREEN
        self.main_traffic_light = agent
        self.got_main = True

    def make_main(self) -> None:
        TrafficLightAgent.there_is_main = True
        self.is_main = True
        self.set_as_main_light(self)

    def get_main(self) -> ap.Agent:
        agent: ap.Agent
        for agent in self.model.grid.agents:
            if agent.type is TrafficLightAgent and agent.is_main:
                self.set_as_main_light(agent)
                break


    def check_for_incoming_cars(self) -> None:
        neighbours: ap.AgentIter = self.model.city.neighbors(self, distance=5)
        neighbour: ap.Agent
        for neighbour in neighbours:
            if neighbour.type is CarAgent:
                if not TrafficLightAgent.there_is_main:
                    self.make_main()
                else:
                    self.get_main()

    def check_status(self):
        if TrafficLightAgent.there_is_main:
            if self.got_main:
                if self.is_main:

                else:
                    if self.current_light == LightColor.YELLOW:

            else:
                self.get_main()
        else:
            if self.current_light == LightColor.YELLOW:
                self.check_for_incoming_cars()
