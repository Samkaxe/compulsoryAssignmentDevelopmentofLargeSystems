"use client";
import { useState } from "react";

export default function Home() {
    const [number1, setNumber1] = useState<number>();
    const [number2, setNumber2] = useState<number>();
    const [result, setResult] = useState<number>();
    const [loading, setLoading] = useState<boolean>(false);

    const handleNumber1 = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setNumber1(value === "" ? undefined : Number(value));
    };
    const handleNumber2 = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        setNumber2(value === "" ? undefined : Number(value));
    };

    const handleSubtract = async () => {
        try {
            setLoading(true);
            const response = await fetch(`https://localhost:7205/Subtraction?number1=${number1}&number2=${number2}`, {
                method: "GET",
            });
            const data = await response.json();
            setResult(data.result);
        } catch (e) {
            console.error(e);
        }
        setLoading(false);
    };

    const handleAdd = async () => {
        try {
            setLoading(true);
            const response = await fetch(`https://localhost:7041/Addition?number1=${number1}&number2=${number2}`, {
                method: "GET",
            });
            const data = await response.json();
            setResult(data.result);
        } catch (e) {
            console.error(e);
        }
        setLoading(false);
    };

    return (
        <main className="flex items-center pt-40 flex-col min-h-screen">
            <div className="w-96">
                <h1>Calculator</h1>
                <div className="flex flex-col mt-5 gap-3">
                    <div>
                        <label htmlFor="number1" className="block text-sm font-medium leading-6 text-gray-900">
                            Number1
                        </label>
                        <div className="mt-2">
                            <input
                                onChange={handleNumber1}
                                value={number1}
                                type="number"
                                name="number1"
                                id="number1"
                                className="px-3 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                                placeholder="Please fill in number"
                            />
                        </div>
                    </div>
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium leading-6 text-gray-900">
                            Number2
                        </label>
                        <div className="mt-2">
                            <input
                                onChange={handleNumber2}
                                value={number2}
                                type="number"
                                name="number2"
                                id="number2"
                                className="px-3 block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                                placeholder="Please fill in number"
                            />
                        </div>
                    </div>
                    <div className="mt-3 mb-3 w-full grid grid-cols-2 gap-3">
                        <button
                            onClick={handleAdd}
                            disabled={loading}
                            type="button"
                            className="disabled:bg-gray-300 rounded-md bg-indigo-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                        >
                            +
                        </button>
                        <button
                            disabled={loading}
                            onClick={handleSubtract}
                            type="button"
                            className="disabled:bg-gray-300 rounded-md bg-indigo-600 px-2.5 py-1.5 text-sm font-semibold text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                        >
                            -
                        </button>
                    </div>
                    <h2>Result</h2>
                    <span>{loading ? "Loading..." : result}</span>
                </div>
            </div>
        </main>
    );
}
