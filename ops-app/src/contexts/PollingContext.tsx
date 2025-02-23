import { createContext, ReactNode, useReducer } from "react";
import { PollAction, PollState } from "../models/Poll";

// Initial state
const initialState: PollState = { polls: [] };

// Reducer function
const pollReducer = (state: PollState, action: PollAction): PollState => {
    switch (action.type) {
        case "ADD_POLL":
            return { ...state, polls: [...state.polls, action.payload] };
        case "VOTE":
            return {
                ...state,
                polls: state.polls.map((poll) =>
                    poll.id === action.payload.pollId && !poll.voted
                        ? {
                            ...poll,
                            votes: poll.votes.map((vote, index) =>
                                index === action.payload.optionIndex ? vote + 1 : vote
                            ),
                            voted: true,
                        }
                        : poll
                ),
            };
        default:
            return state;
    }
};

// Create context
export const PollContext = createContext<{
    state: PollState;
    dispatch: React.Dispatch<PollAction>;
} | null>(null);

// Provider component
export const PollProvider = ({ children }: { children: ReactNode }) => {
    const [state, dispatch] = useReducer(pollReducer, initialState);

    return (
        <PollContext.Provider value={{ state, dispatch }}>
            {children}
        </PollContext.Provider>
    );
};