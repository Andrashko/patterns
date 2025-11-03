from patterns._3_5_1_visitor import Professor, Student


class PrintPythonicVisitor:  # (IVisitor)
    def visit(self, target: Student | Professor) -> None:
        match target:
            case Student():
                print("Друкую студента")
                print(f"Курс {target.course}, {target.name} {target.surname}")
            case Professor():
                print("Друкую професора")
                print(
                    f"{target.surname} {target.name} {target.secondname}, {target.cathedra}")
            case _:
                raise TypeError


class SayHiPythonicVisitor:  # (IVisitor)
    def visit(self, target: Student | Professor) -> None:
        match target:
            case Student():
                print(f"Привіт, {target.name}!")
            case Professor():
                print(f"Доброго дня, {target.name} {target.secondname}.")
            case _:
                raise TypeError
