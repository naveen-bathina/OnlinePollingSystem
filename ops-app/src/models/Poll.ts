export interface PollOption {
    id: number;
    pollId: number;
    optionText: string;
    voteCount: number;
}

export interface Poll {
    id: number;
    question: string;
    options: PollOption[];
    hasVoted: boolean;
    votedOptionId?: number;
}

export interface CreatePoll {
    question: string;
    options: string[];
}