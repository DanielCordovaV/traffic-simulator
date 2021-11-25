import enum


# y, x
class Direction(enum.Enum):
    LEFT = (0, -1)
    RIGHT = (0, 1)
    UP = (1, 0)
    DOWN = (-1, 0)


class Road:
    def __init__(self, direction: Direction) -> None:
        self.directions = [direction]

    def add_direction(self, direction: Direction) -> None:
        self.directions.append(direction)


class City:
    def __init__(self, height: int, width: int) -> None:
        self.height = height
        self.width = width
        self.amount_of_roads = 0

        self.grid = [[None for y in range(height)] for x in range(width)]

    def add_street(self, start: tuple, end: tuple, direction: Direction) -> None:
        for i in range(start[0], end[0]):
            print(i)
            road = self.grid[i][start[1]]

            if type(road) is Road:
                self.grid[i][start[1]].add_direction(direction)
            else:
                self.grid[i][start[1]] = Road(direction)
                self.amount_of_roads += 1
        for i in range(start[1], end[1]):
            print(i)
            road = self.grid[start[0]][i]

            if type(road) is Road:
                self.grid[start[0]][i].add_direction(direction)
            else:
                self.grid[start[0]][i] = Road(direction)
                self.amount_of_roads += 1

    def __str__(self):
        return str(self.grid)


if __name__ == "__main__":
    city = City(5, 5)
    print(city)
    city.add_street((1, 1), (1, 4), Direction.RIGHT)
    city.add_street((0, 1), (3, 1), Direction.UP)
    print(city)
