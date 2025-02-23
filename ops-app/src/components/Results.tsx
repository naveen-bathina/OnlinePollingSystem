import React, { useEffect, useState } from 'react';
import { Card, Typography, Box, Button, Grid } from '@mui/material';
import { useParams, useNavigate } from 'react-router-dom';
import PollApiService from '../services/PollApiService';
import { Poll } from '../models/Poll';
import { BarChart, Bar, XAxis, YAxis, Tooltip, PieChart, Pie, Cell, ResponsiveContainer } from 'recharts';

const COLORS = ['#8884d8', '#82ca9d', '#ffc658', '#ff7f50', '#ff69b4'];

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
                setPoll(fetchedPoll);
                setTotalVotes(fetchedPoll.options.reduce((acc, option) => acc + option.voteCount, 0));
            } catch (error) {
                console.error('Error fetching poll:', error);
            }
        };
        fetchPollResult();
    }, [pollId]);

    const chartData = poll?.options.map(option => ({
        name: option.optionText,
        votes: option.voteCount,
    })) || [];

    return (
        <Box sx={{ mt: 5, textAlign: 'center' }}>
            <Typography variant="h4" gutterBottom>
                Poll Results: {poll?.question}
            </Typography>
            {totalVotes === 0 ? (
                <Typography variant="h6" color="textSecondary">
                    No votes yet.
                </Typography>
            ) : (
                <Grid container spacing={3} justifyContent="center">
                    {/* Bar Chart */}
                    <Grid item xs={12} md={6}>
                        <Card sx={{ boxShadow: 3, padding: 2 }}>
                            <Typography variant="h6">Vote Distribution</Typography>
                            <ResponsiveContainer width="100%" height={300}>
                                <BarChart data={chartData} layout="vertical">
                                    <XAxis type="number" allowDecimals={false} />
                                    <YAxis type="category" dataKey="name" width={100} />
                                    <Tooltip />
                                    <Bar dataKey="votes" fill="#8884d8" barSize={30} />
                                </BarChart>
                            </ResponsiveContainer>
                        </Card>
                    </Grid>

                    {/* Pie Chart */}
                    <Grid item xs={12} md={6}>
                        <Card sx={{ boxShadow: 3, padding: 2 }}>
                            <Typography variant="h6">Vote Percentage</Typography>
                            <ResponsiveContainer width="100%" height={300}>
                                <PieChart>
                                    <Pie data={chartData} dataKey="votes" nameKey="name" cx="50%" cy="50%" outerRadius={100} label>
                                        {chartData.map((_, index) => (
                                            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                                        ))}
                                    </Pie>
                                    <Tooltip />
                                </PieChart>
                            </ResponsiveContainer>
                        </Card>
                    </Grid>
                </Grid>
            )}
            <Box sx={{ mt: 4 }}>
                <Button variant="outlined" color="primary" onClick={() => navigate('/')}>Back</Button>
            </Box>
        </Box>
    );
};

export default Results;
