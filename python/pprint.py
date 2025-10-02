class Symbol:
    def get_template_row(self, row: int) -> str:
        ...


class One(Symbol):
    def __init__(self) -> None:
        self.template = ["***  ", "  *  ",  "  *  ", "  *  ", "*****"]


class Number:
    def __init__(self, value: str) -> None:
        self.symbols = []
        for digit in value:
            if digit == "1":
                self.symbols.append(One())

    def pprint(self):
        for row in range(5):
            string = ""
            for symbol in self.symbols:
                string += symbol.template[row]
            print(string)

Number("111").pprint()
