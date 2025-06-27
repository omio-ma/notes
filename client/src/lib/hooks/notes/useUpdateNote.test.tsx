import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { renderHook, waitFor } from "@testing-library/react";
import * as notesApi from "../../api/notes/notes";
import { useUpdateNote } from "./useUpdateNote";
import type { Note } from "../../types";

jest.mock("../../api/notes/notes");
jest.mock("../../agent");

const mockedUpdateNote = notesApi.updateNote as jest.MockedFunction<
  typeof notesApi.updateNote
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

describe("useUpdateNote", () => {
  it("updates a note and updates the cached note list", async () => {
    const originalNote: Note = {
      id: 1,
      title: "Old title",
      content: "Old content",
      createdAt: new Date().toISOString()
    };

    const updatedNote: Note = {
      ...originalNote,
      title: "Updated title",
      content: "Updated content"
    };

    mockedUpdateNote.mockResolvedValueOnce(updatedNote);

    const { queryClient, wrapper } = createWrapperWithPreloadedCache([
      originalNote
    ]);

    const { result } = renderHook(() => useUpdateNote(), { wrapper });

    result.current.mutate({
      id: 1,
      data: {
        title: "Updated title",
        content: "Updated content"
      }
    });

    await waitFor(() => {
      const updatedCache = queryClient.getQueryData<Note[]>(["notes"]);
      expect(updatedCache).toBeDefined();
      expect(updatedCache).toHaveLength(1);
      expect(updatedCache?.[0].title).toBe("Updated title");
      expect(updatedCache?.[0].content).toBe("Updated content");
    });
  });
});
