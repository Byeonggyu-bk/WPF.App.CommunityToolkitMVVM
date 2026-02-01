import yaml
import random
import csv
from datetime import datetime
from pathlib import Path
import sys

def base_dir():
    if getattr(sys, 'frozen', False):
        return Path(sys.executable).parent
    return Path(__file__).resolve().parent

ROOT = base_dir()
TESTS_YAML = ROOT / "tests.yaml"

def measure(test_type):
    if test_type == "voltage":
        return random.uniform(4.6, 5.4)
    if test_type == "current":
        return random.uniform(0.05, 0.45)
    if test_type == "boolean":
        return random.random() > 0.1
    raise ValueError("Unknown type")


def main():
    with open(TESTS_YAML, "r", encoding="utf-8") as f:
        config = yaml.safe_load(f)

    results = []

    for test in config["tests"]:
        value = measure(test["type"])

        if test["type"] == "boolean":
            passed = value == test["expect"]
        else:
            passed = test["min"] <= value <= test["max"]

        results.append({
            "name": test["name"],
            "type": test["type"],
            "value": value,
            "passed": passed
        })

        print(test["name"], value, "PASS" if passed else "FAIL")

    filename = f"result_{datetime.now().strftime('%Y%m%d_%H%M%S')}.csv"

    with open(filename, "w", newline="") as f:
        writer = csv.DictWriter(f, results[0].keys())
        writer.writeheader()
        writer.writerows(results)

    print("\nSaved:", filename)


if __name__ == "__main__":
    main()
