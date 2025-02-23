import React, { useEffect, useState } from 'react';
import { Card, CardContent, Typography, LinearProgress, Box, Button } from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import PollApiService from '../services/PollApiService';
import { Poll } from '../models/Poll';

const Results: React.FC = () => {
    const { pollId } = useParams<{ pollId: string }>();
    const navigate = useNavigate();
    const [poll, setPoll] = useState<Poll>();
    const [totalVotes, setTotalVotes] = useState(0);

    useEffect(() => {
        const pollService = new PollApiService('https://localhost:7262/api');
        const fetchPollResult = async () => {
            try {
                const fetchedPoll = await pollService.getPollResults(Number(pollId));
                console.log('Fetched poll:', fetchedPoll);
                setPoll(fetchedPoll);
                setTotalVotes(fetchedPoll.options.reduce((acc, votes) => acc + votes.voteCount, 0));
            } catch (error) {
                console.error('Error fetching poll:', error);
            }
        };

        fetchPollResult();
    }, [pollId]);

    return (
        <div className="container mt-5">
            <Typography variant="h4" align="center" gutterBottom>
                Poll Results: {poll?.question}
            </Typography>
            {totalVotes === 0 ? (
                <Typography variant="h6" align="center" color="textSecondary">
                    No votes yet.
                </Typography>
            ) : (
                poll?.options.map((option, index) => {
                    const percentage = totalVotes > 0 ? ((poll?.options[index].voteCount || 0) / totalVotes) * 100 : 0;
                    return (
                        <Card key={index} sx={{ marginBottom: 2, boxShadow: 3 }}>
                            <CardContent>
                                <Typography variant="h6">{option.optionText}</Typography>
                                <Box sx={{ width: '100%' }}>
                                    <LinearProgress
                                        variant="determinate"
                                        value={percentage}
                                        sx={{ marginBottom: 1, backgroundColor: '#FFEBEE' }}
                                    />
                                    <Typography variant="body2" align="right">
                                        {Math.round(percentage)}% ({poll.options[index].voteCount} votes)
                                    </Typography>
                                </Box>
                            </CardContent>
                        </Card>
                    );
                })
            )}
            <div className="text-center mt-4">
                <Button variant="outlined" color="primary" onClick={() => navigate('/')}>
                    Back
                </Button>
            </div>
        </div>
    );
};

export default Results;
