from test_patterns.creational import test_classic_singleton, test_pythonic_singleton, test_metaclass_singleton, test_logger_singleton

def main():
    test_classic_singleton()
    test_pythonic_singleton()
    test_metaclass_singleton()
    test_logger_singleton()

if __name__ == "__main__":
    main()