import enum


class Orientation(enum.Enum):
    HORIZONTAL = 'h'
    VERTICAL = 'v'


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
        self.grid: list[list] = [[None for _ in range(height)] for _ in range(width)]

    def add_street(self, start: tuple[int, int], end: tuple[int, int], direction: Direction) -> None:
        start_x, start_y = start
        end_x, end_y = end
        if direction in (Direction.RIGHT, Direction.LEFT):  # Horizontal street
            for i in range(start_y, end_y):
                if self.grid[start_x][i] is None:
                    self.grid[start_x][i] = Road(direction)
                    self.amount_of_roads += 1
                # There is already a Road crossing that square
                elif type(self.grid[start_x][i]) is Road:
                    self.grid[start_x][i].add_direction(direction)

        else:  # Vertical street
            for i in range(start_x, end_x):
                if self.grid[i][start_y] is None:
                    self.grid[i][start_y] = Road(direction)
                    self.amount_of_roads += 1
                elif type(self.grid[i][start_y]) is Road:
                    self.grid[i][start_y].add_direction(direction)

    def __str__(self):
        return str(self.grid)
