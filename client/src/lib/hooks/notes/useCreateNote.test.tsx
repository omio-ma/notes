import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { renderHook, waitFor } from "@testing-library/react";
import * as notesApi from "../../api/notes/notes";
import type { Note } from "../../types";
import { useCreateNote } from "./useCreateNote";
import type { AxiosResponse } from "axios";

jest.mock("../../api/notes/notes");
jest.mock("../../agent");

const mockedCreateNote = notesApi.createNote as jest.MockedFunction<
  typeof notesApi.createNote
>;

function createWrapperWithPreloadedCache(initialNotes: Note[]) {
  const queryClient = new QueryClient();
  queryClient.setQueryData<Note[]>(["notes"], initialNotes);

  return {
    queryClient,
    wrapper: ({ children }: { children: React.ReactNode }) => (
      <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
    )
  };
}

describe("useCreateNote", () => {
  it("creates a new note and invalidates the cache", async () => {
    const initialNotes: Note[] = [
      {
        id: 1,
        title: "Note 1",
        content: "Content 1",
        createdAt: new Date().toISOString()
      }
    ];

    const requestPayload = {
      title: "New Note",
      content: "New Content"
    };

    const mockResponse = {
      data: undefined,
      status: 201,
      statusText: "Created",
      headers: {
        location: "/notes/14"
      },
      config: {}
    };

    mockedCreateNote.mockResolvedValueOnce(
      mockResponse as unknown as AxiosResponse<Note>
    );

    const { queryClient, wrapper } =
      createWrapperWithPreloadedCache(initialNotes);

    const invalidateSpy = jest.spyOn(queryClient, "invalidateQueries");

    const { result } = renderHook(() => useCreateNote(), { wrapper });

    result.current.mutate(requestPayload);

    await waitFor(() => {
      expect(mockedCreateNote).toHaveBeenCalledWith(requestPayload);
      expect(invalidateSpy).toHaveBeenCalledWith({ queryKey: ["notes"] });
    });
  });
});
