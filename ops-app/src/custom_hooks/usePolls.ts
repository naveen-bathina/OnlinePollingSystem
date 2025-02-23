import { useContext } from "react";
import { PollContext } from "../contexts/PollingContext";

export const usePolls = () => {
    const context = useContext(PollContext);
    if (!context) {
        throw new Error("usePolls must be used within a PollProvider");
    }
    return context;
};
