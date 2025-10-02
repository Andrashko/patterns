class Symbol:
    def __init__(self) -> None:
        self.template: list[str] = []


class One(Symbol):
    def __init__(self) -> None:
        self.template: list[str] = [
            "***  ", "  *  ",  "  *  ", "  *  ", "*****"]


class Number:
    def __init__(self, value: str) -> None:
        self.symbols: list[Symbol] = []
        for digit in value:
            if digit == "1":
                self.symbols.append(One())

    def print(self) -> None:
        for row in range(5):
            string: str = ""
            for symbol in self.symbols:
                string += symbol.template[row]
            print(string)


Number("1111").print()
