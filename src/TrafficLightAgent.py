import enum
import agentpy as ap
import CarAgent
from City import Orientation, Direction


class LightColor(enum.Enum):
    GREEN = 'green_light'
    YELLOW = 'yellow_light'
    RED = 'red_light'


class TrafficLightAgent(ap.Agent):
    MAX_TIMER = 10  # Max steps per green light
    there_is_main = False

    def setup(self, direction: tuple[int, int]) -> None:
        self.__current_light = LightColor.YELLOW
        self.__timer = 0
        self.is_main = False
        self.got_main = False
        main_traffic_light: TrafficLightAgent
        self.direction = direction
        if direction == Direction.DOWN.value or direction == Direction.UP.value:
            self.orientation = Orientation.VERTICAL
        else:
            self.orientation = Orientation.HORIZONTAL

    def get_current_light(self) -> LightColor:
        return self.__current_light

    def reset_timer(self):
        self.__timer = 0

    def set_as_main_light(self, agent: ap.Agent) -> None:
        self.__current_light = LightColor.GREEN
        self.main_traffic_light = agent
        self.got_main = True

    def make_main(self) -> None:
        TrafficLightAgent.there_is_main = True
        self.is_main = True
        self.set_as_main_light(self)

    def get_main(self) -> None:
        agent: ap.Agent
        for agent in self.model.grid.agents:
            if agent.type is TrafficLightAgent and agent.is_main:
                self.set_as_main_light(agent)
                break

    def check_for_incoming_cars(self) -> None:
        neighbours: ap.AgentIter = self.model.grid.neighbors(self, distance=6)
        neighbour: ap.Agent
        for neighbour in neighbours:
            if neighbour.type == 'CarAgent':
                if not TrafficLightAgent.there_is_main:
                    self.make_main()
                else:
                    self.get_main()

    def operate_as_main(self):
        if self.__timer < TrafficLightAgent.MAX_TIMER:
            self.__timer += 1
        else:
            if self.__current_light is LightColor.GREEN:
                self.__current_light = LightColor.RED
            else:
                self.__current_light = LightColor.GREEN
            self.reset_timer()

    def operate_as_slave(self):
        if self.orientation == self.main_traffic_light.orientation:
            self.__current_light = self.main_traffic_light.get_current_light()
        else:
            if self.main_traffic_light.get_current_light() == LightColor.GREEN:
                self.__current_light = LightColor.RED
            else:
                self.__current_light = LightColor.GREEN

    def check_status(self):
        if TrafficLightAgent.there_is_main:
            if self.got_main:
                if self.is_main:
                    self.operate_as_main()
                else:
                    self.operate_as_slave()
            else:
                self.get_main()
        else:
            self.check_for_incoming_cars()
