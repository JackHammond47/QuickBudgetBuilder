import { useState } from "react";
import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";

ChartJS.register(ArcElement, Tooltip, Legend);

export default function App() {
  const [monthlyIncome, setMonthlyIncome] = useState("");
  const [rent, setRent] = useState("");
  const [lifestyle, setLifestyle] = useState("modest");
  const [zipCode, setZipCode] = useState("");
  const [result, setResult] = useState(null);
  const [error, setError] = useState("");
  const [adults, setAdults] = useState(1);
  const [children, setChildren] = useState(0);
  const [needsChildcare, setNeedsChildcare] = useState(false);

  async function handleCalculate(e) {
    e.preventDefault();
    setError("");
    setResult(null);

    try {
      const response = await fetch("https://localhost:7219/api/Budget/calculate", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          monthlyIncome: Number(monthlyIncome),
          rent: Number(rent),
          lifestyle: lifestyle,
          zipCode: zipCode,
          adults: Number(adults),
          children: Number(children),
          needsChildcare: needsChildcare
        })
      });

      if (!response.ok) {
        throw new Error("Request failed.");
      }

      const data = await response.json();
      setResult(data);
    } catch (err) {
      setError("Could not connect to backend.");
      console.error(err);
    }
  }

  function handleExport() {
  const rows = [
    ["Category", "Amount"],
    ["Housing", result.housing],
    ["Utilities", result.utilities],
    ["Groceries", result.groceries],
    ["Eating Out", result.eatingOut],
    ["Transportation", result.transportation],
    ["Health", result.health],
    ["Insurance", result.insurance],
    ["Savings", result.savings],
    ["Entertainment", result.entertainment],
    ["Personal Spending", result.personalSpending],
    ["Childcare", result.childcare],
    ["Miscellaneous", result.miscellaneous],
    ["Remaining", result.remaining],
    ["Investments / Extra Savings", result.investments],
    ["Budget Deficit", result.debt],
  ];

  const csvContent = rows.map(r => r.join(",")).join("\n");
  const blob = new Blob([csvContent], { type: "text/csv" });
  const url = URL.createObjectURL(blob);

  const a = document.createElement("a");
  a.href = url;
  a.download = "budget.csv";
  a.click();

  URL.revokeObjectURL(url);
}

  return (
    <div
      style={{
        maxWidth: "600px",
        margin: "60px auto",
        padding: "20px",
        fontFamily: "Arial",
        backgroundColor: "#f9f9f9",
        borderRadius: "12px",
        boxShadow: "0 4px 10px rgba(0,0,0,0.1)"
      }}
    >
      <h1>Quick Budget Builder</h1>

      <form onSubmit={handleCalculate} style={{ display: "grid", gap: "12px" }}>
        <div>
          <label>Monthly Income</label>
          <input
            type="number"
            value={monthlyIncome}
            onChange={(e) => setMonthlyIncome(e.target.value)}
            style={{ width: "100%", padding: "8px" }}
            required
          />
        </div>

        <div>
          <label>Housing Costs (Rent/Mortgage)</label>
          <input
            type="number"
            value={rent}
            onChange={(e) => setRent(e.target.value)}
            style={{ width: "100%", padding: "8px" }}
            required
          />
        </div>

        <div>
          <label>Lifestyle</label>
          <select
            value={lifestyle}
            onChange={(e) => setLifestyle(e.target.value)}
            style={{ width: "100%", padding: "8px" }}
          >
            <option value="bare-bones">Bare-Bones</option>
            <option value="modest">Modest</option>
            <option value="comfortable">Comfortable</option>
            <option value="high-spending">High-Spending</option>
          </select>
        </div>

        <div>
          <label>ZIP Code</label>
          <input
            type="text"
            value={zipCode}
            onChange={(e) => setZipCode(e.target.value)}
            style={{ width: "100%", padding: "8px" }}
            required
          />
        </div>

        <div>
          <label>Adults in Household</label>
          <select
            value={adults}
            onChange={(e) => setAdults(e.target.value)}
            style={{ width: "100%", padding: "8px" }}
          >
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
          </select>
        </div>

        <div>
          <label>Children in Household</label>
          <select
            value={children}
            onChange={(e) => setChildren(e.target.value)}
            style={{ width: "100%", padding: "8px" }}
          >
            <option value="0">0</option>
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
          </select>
        </div>

        <div>
          <label>
            <input
              type="checkbox"
              checked={needsChildcare}
              onChange={(e) => setNeedsChildcare(e.target.checked)}
            />
            Needs Childcare
          </label>
        </div>

        <button type="submit" style={{ padding: "10px" }}>
          Calculate Budget
        </button>
      </form>

      {error && <p style={{ color: "red" }}>{error}</p>}

      {result && (
        <div style={{ marginTop: "20px" }}>
          <h2>Suggested Budget</h2>
          <button onClick={handleExport} style={{ padding: "8px 16px", marginBottom: "12px" }}>
            Export to CSV
          </button>
          <Pie
            data={{
              labels: [
                "Housing",
                "Utilities",
                "Groceries",
                "Eating Out",
                "Transportation",
                "Health",
                "Insurance",
                "Savings",
                "Entertainment",
                "Personal Spending",
                "Childcare",
                "Miscellaneous",
              ],
              datasets: [
                {
                  data: [
                    result.housing,
                    result.utilities,
                    result.groceries,
                    result.eatingOut,
                    result.transportation,
                    result.health,
                    result.insurance,
                    result.savings,
                    result.entertainment,
                    result.personalSpending,
                    result.childcare,
                    result.miscellaneous,
                  ],
                  backgroundColor: [
                    "#FF6384",
                    "#36A2EB",
                    "#FFCE56",
                    "#4BC0C0",
                    "#9966FF",
                    "#FF9F40",
                    "#E7E9ED",
                    "#76D7C4",
                    "#F1948A",
                    "#85C1E9",
                    "#F8C471",
                    "#ABEBC6",
                  ],
                },
              ],
            }}
            options={{
              plugins: {
                legend: {
                  position: "bottom",
                },
              },
            }}
          />
          <p
            style={{
              color: result.housingOverBudget ? "red" : "black",
              fontWeight: result.housingOverBudget ? "bold" : "normal"
            }}
          >
            Housing: ${result.housing.toFixed(2)}
          </p>

          {result.housingOverBudget && (
            <p style={{ color: "red" }}>
              Suggested: ${result.suggestedHousing.toFixed(2)} (25% of income)
            </p>
          )}
          <p>Utilities: ${result.utilities.toFixed(2)}</p>
          <p>Groceries: ${result.groceries.toFixed(2)}</p>
          <p>Eating Out: ${result.eatingOut.toFixed(2)}</p>
          <p>Transportation: ${result.transportation.toFixed(2)}</p>
          <p>Health: ${result.health.toFixed(2)}</p>
          <p>Insurance: ${result.insurance.toFixed(2)}</p>
          <p>Savings: ${result.savings.toFixed(2)}</p>
          <p>Entertainment: ${result.entertainment.toFixed(2)}</p>
          <p>Personal Spending: ${result.personalSpending.toFixed(2)}</p>
          <p>Childcare: ${result.childcare.toFixed(2)}</p>
          <p>Miscellaneous: ${result.miscellaneous.toFixed(2)}</p>
          <p>Remaining: ${result.remaining.toFixed(2)}</p>

          {result.investments > 0 && (
            <p style={{ color: "green" }}>
              Investments / Extra Savings: ${result.investments.toFixed(2)}
            </p>
          )}

          {result.debt > 0 && (
            <p style={{ color: "red" }}>
              Budget Deficit (Debt): ${result.debt.toFixed(2)}
            </p>
          )}

          <h3>Disclaimers</h3>
          <ul>
            {result.disclaimers.map((text, index) => (
              <li key={index}>{text}</li>
            ))}
          </ul>
        </div>
      )}
    </div>
  );
}
