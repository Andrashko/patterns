class Symbol:
    def __init__(self) -> None:
        self.template: list[str] = []


class One(Symbol):
    def __init__(self) -> None:
        self.template = [
            "***  ",
            "  *  ",
            "  *  ",
            "  *  ",
            "*****"]

class Two(Symbol):
    def __init__(self) -> None:
        self.template = [
            "*****",
            "    *",
            "*****",
            "*    ",
            "*****"]

class ConsoleSymbolNumber:
    def __init__(self, value: str) -> None:
        self.symbols: list[Symbol] = []
        for digit in value:
            if digit == "1":
                self.symbols.append(One())
            elif digit == "2":
                self.symbols.append(Two())


    def print(self) -> None:
        for row in range(5):
            string: str = ""
            for symbol in self.symbols:
                string += symbol.template[row] + " "
            print(string)


ConsoleSymbolNumber("12112").print()
