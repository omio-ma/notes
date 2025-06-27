import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { renderHook, waitFor } from "@testing-library/react";
import * as notesApi from "../../api/notes/notes";
import type { Note } from "../../types";
import { useNotes } from "./useNotes";

jest.mock("../../api/notes/notes");
jest.mock("../../agent");

const mockedGetAllNotes = notesApi.getAllNotes as jest.MockedFunction<
  typeof notesApi.getAllNotes
>;

function createWrapper() {
  const queryClient = new QueryClient();
  return ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
}

describe("useNotes", () => {
  it("returns notes from the API", async () => {
    const fakeNotes: Note[] = [
      {
        id: 1,
        title: "Test Note",
        content: "This is a test note",
        createdAt: new Date().toISOString()
      }
    ];

    mockedGetAllNotes.mockResolvedValueOnce(fakeNotes);

    const { result } = renderHook(() => useNotes(), {
      wrapper: createWrapper()
    });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toEqual(fakeNotes);
  });
});
