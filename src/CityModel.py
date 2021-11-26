import agentpy as ap
from RoadAgent import RoadAgent
from TrafficLightAgent import TrafficLightAgent
from CarAgent import CarAgent
from City import City, Road
from random import choice


class CityModel(ap.Model):
    @staticmethod
    def __get_positions(grid: list[list]) -> list[tuple[int, int]]:
        positions = []
        for i, column in enumerate(grid):
            for j, space in enumerate(column):
                if type(space) is Road:
                    positions.append((i, j))
        return positions

    def setup(self):
        # Unpack parameters
        self.city: City = self.p["city"]
        self.n_cars: int = self.p["cars"]
        self.spawn_points: list[tuple[int, int]] = self.p["spawn_points"]
        self.size: tuple[int, int] = self.p["size"]

        # Create grid (city)
        self.grid = ap.Grid(self, self.size, track_empty=True, torus=True)

        # Create agents
        self.__add_roads()  # Roads must be added before traffic lights
        self.__add_traffic_lights()
        self.__add_cars()

    def step(self):
        cars_to_delete = ap.AgentList(self, cls=CarAgent)
        for agent in self.grid.agents:
            if agent.type == "CarAgent":
                direction = agent.select_next_direction()
                if agent.check_if_can_move(direction):
                    if agent.check_for_traffic_light(direction):
                        agent.move(direction)
                else:
                    cars_to_delete.append(agent)
            elif agent.type == "TrafficLightAgent":
                agent.check_status()
        self.grid.remove_agents(cars_to_delete)

    def update(self):
        pass

    def end(self):
        pass

    def __add_roads(self) -> None:
        roads = ap.AgentList(self, self.city.amount_of_roads, RoadAgent)
        road_positions = self.__get_positions(self.city.grid)
        self.grid.add_agents(roads, road_positions)
        for road in self.grid.agents:
            position = self.grid.positions[road]
            x, y = position
            road.add_directions(self.city.grid[x][y].directions)

    def __add_traffic_lights(self) -> None:
        intersections = ap.AgentList(self, cls=RoadAgent)
        for road in self.grid.agents:
            if len(road.directions) > 1:  # There is an intersection
                intersections.append(road)
        for road in intersections:
            traffic_lights = ap.AgentList(self, cls=TrafficLightAgent)
            for direction in road.directions:
                traffic_lights.append(TrafficLightAgent(self, direction))
            self.grid.add_agents(traffic_lights, [tuple(self.grid.positions[road])] * len(road.directions))

    def __add_cars(self) -> None:
        cars = ap.AgentList(self, self.n_cars, CarAgent)
        self.grid.add_agents(cars, positions=[choice(self.spawn_points) for _ in range(self.n_cars)])
