import axios, { AxiosInstance } from 'axios';
import { CreatePoll, Poll } from '../models/Poll';
import cookies from 'js-cookie';

class PollApiService {
    private baseUrl: string;
    private deviceId: string;

    constructor(baseUrl: string) {
        this.baseUrl = baseUrl;
        this.deviceId = cookies.get('deviceId') || '';
    }

    getAxios = (): AxiosInstance => {
        return axios.create({
            baseURL: 'https://localhost:7262/api',
            headers: {
                'Device-Id': this.deviceId,
                'Content-Type': 'application/json',
            },
            withCredentials: true,
        });
    };

    async getPolls(): Promise<Poll[]> {
        const response = await this.getAxios().get(`${this.baseUrl}/polls`);
        return response.data;
    }

    async getPoll(id: number): Promise<Poll> {
        const response = await this.getAxios().get(`${this.baseUrl}/polls/${id}`);
        return response.data;
    }

    async createPoll(poll: CreatePoll): Promise<string> {
        const response = await this.getAxios().post(`${this.baseUrl}/polls`, poll);
        return response.data;
    }

    async votePoll(pollId: number, optionIndex: number): Promise<void> {
        await this.getAxios().post(`${this.baseUrl}/polls/${pollId}/vote`, { optionId: optionIndex });
    }

    async getPollResults(pollId: number): Promise<Poll> {
        const response = await this.getAxios().get(`${this.baseUrl}/polls/${pollId}/results`);
        return response.data;
    }

    async resetPoll(pollId: number): Promise<boolean> {
        const response = await this.getAxios().post(`${this.baseUrl}/polls/${pollId}/reset`);
        return response.status === 200;
    }
}

export default PollApiService;
